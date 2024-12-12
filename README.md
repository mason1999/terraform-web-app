# Terraform web app

## Provisioning the example

To provision the example:

1. Fill in the following values in a `terraform.tfvars` file:

   ```hcl
   web_app_resource_group_name  = "test-rg"
   web_app_storage_account_name = "masonwebappstore00000000"
   table_storage_account_name   = "masontablestoretodo01"
   public_access_enabled        = true
   file_share_name              = "web-app-content"
   ```

1. Run `az login`
1. Ensure that the following environment variable is filled in: `export ARM_SUBSCRIPTION_ID=<subscription id>`.
1. Run `terraform apply -auto-approve`.
1. Run the `./upload-git.sh` script.

## THINGS TO BE DONE

- Refactor to use managed identities
- Draw diagram
- Make an api that will cause an exception
- Configure application insights
