terraform {
  required_version = ">=1.7"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">=3.1"
    }
  }
}

provider "azurerm" {
  features {}
  # export ARM_SUBSCRIPTION_ID
}
