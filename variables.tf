variable "web_app_resource_group_name" {
  description = "(Required) The name of the resource group to create the resources for the web app."
  type        = string
}

variable "shared_resource_group_name" {
  description = "(Required) The name of the shared resource group to create and reference shared resources (like databases, keyvaults and virtual networks etc)."
  type        = string
}

variable "location" {
  description = "(Optional) The Azure region to place your resources in. Defaults to australiaeast."
  type        = string
  default     = "australiaeast"
}

variable "web_app_storage_account_name" {
  description = "(Required) The name of the storage account to used by the web app."
  type        = string
}

variable "table_storage_account_name" {
  description = "(Required) The name of the storage account to used by the web app as a database (using table storage)."
  type        = string
}

variable "public_access_enabled" {
  description = "(Required) Enable public access (or not) on all the resources."
  type        = bool
}

variable "file_share_name" {
  description = "(Required) The name of the file share to be referenced in the storage account of the web app."
  type        = string
}

variable "tags" {
  description = "(Optional) The tags to assign to each infrastructure resource."
  type        = map(string)
  default     = {}
}
