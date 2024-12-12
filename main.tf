################################################################################
# Web App Storage Account
################################################################################
resource "azurerm_storage_account" "table_storage_database" {
  name                            = var.table_storage_account_name
  resource_group_name             = var.web_app_resource_group_name
  location                        = var.location
  account_tier                    = "Standard"
  account_replication_type        = "LRS"
  shared_access_key_enabled       = true
  default_to_oauth_authentication = true
  public_network_access_enabled   = var.public_access_enabled
  tags                            = var.tags
}

################################################################################
# Web App Storage Account
################################################################################
resource "azurerm_storage_account" "web_app_storage_account" {
  name                            = var.web_app_storage_account_name
  resource_group_name             = var.web_app_resource_group_name
  location                        = var.location
  account_tier                    = "Standard"
  account_replication_type        = "LRS"
  shared_access_key_enabled       = true
  default_to_oauth_authentication = true
  public_network_access_enabled   = var.public_access_enabled
  tags                            = var.tags
  provisioner "local-exec" {
    when    = create
    command = <<EOF
    az storage share-rm create \
      --resource-group ${var.web_app_resource_group_name} \
      --storage-account ${self.name} \
      --name ${var.file_share_name} \
      --quota 100000
    EOF
  }
}

################################################################################
# App Service Plan
################################################################################
resource "azurerm_service_plan" "app_service_plan_web_app" {
  name                = "app-service-plan-web-app"
  resource_group_name = var.web_app_resource_group_name
  location            = var.location
  os_type             = "Linux"
  sku_name            = "P1v2"
  worker_count        = 3
  tags                = var.tags
}

################################################################################
# App Service - Web App
################################################################################
resource "azurerm_linux_web_app" "web_app" {
  name                = "web-app-mason-0000000"
  location            = var.location
  resource_group_name = var.web_app_resource_group_name
  service_plan_id     = azurerm_service_plan.app_service_plan_web_app.id
  site_config {
    worker_count = 4
    application_stack {
      dotnet_version = "8.0"
    }
  }
  app_settings = {
    TableStorage = azurerm_storage_account.table_storage_database.primary_connection_string
  }
  storage_account {
    access_key   = azurerm_storage_account.web_app_storage_account.primary_access_key
    account_name = azurerm_storage_account.web_app_storage_account.name
    share_name   = var.file_share_name
    type         = "AzureFiles"
    name         = "test-name"
  }
}

resource "azurerm_app_service_source_control" "this" {
  app_id        = azurerm_linux_web_app.web_app.id
  use_local_git = true
}

output "site_credential" {
  value     = azurerm_linux_web_app.web_app.site_credential
  sensitive = true
}

output "username" {
  value     = azurerm_linux_web_app.web_app.site_credential[0].name
  sensitive = true
}

output "password" {
  value     = azurerm_linux_web_app.web_app.site_credential[0].password
  sensitive = true
}

output "git_clone_uri" {
  value = "https://${azurerm_linux_web_app.web_app.name}.scm.azurewebsites.net:443/${azurerm_linux_web_app.web_app.name}.git"
}
