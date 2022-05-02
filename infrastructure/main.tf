locals {
  name       = "memetrics-${terraform.workspace}"
  short_name = "mm${lower(replace(terraform.workspace, "-", ""))}"
}

data "azurerm_subscription" "current" {}

locals {
  # Double underscore (__) is used due to using Linux to host our apps
  # Which use double underscore for nested app settings instead of colon (:)
  api_app_settings = {
    MSDEPLOY_RENAME_LOCKED_FILES                                = "1"
    DB_CONNECTION_STRING                                        = var.db_connection_string
    env                                                         = var.env
    log_level                                                   = var.log_level
    PRIMARY_API_KEY                                             = var.primary_api_key
    SECONDARY_API_KEY                                           = var.secondary_api_key
    # This is a special name for the key https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core#code-try-2
    APPINSIGHTS_INSTRUMENTATIONKEY                              = var.app_insights_intrumentation_key
    DOCKER_CUSTOM_IMAGE_NAME                                    = var.docker_api_custom_image_name
    DOCKER_REGISTRY_SERVER_URL                                  = var.docker_registry_server_url
    DOCKER_REGISTRY_SERVER_USERNAME                             = var.docker_registry_server_username
    DOCKER_REGISTRY_SERVER_PASSWORD                             = var.docker_registry_server_password
    ASPNETCORE_ENVIRONMENT                                      = var.aspnetcore_environment
    ALLOWED_ORIGIN                                              = var.allowed_origin

    BLOB_STORAGE_CONNECTION_STRING                              = var.blob_storage_connection_string
    GOOGLE_CLIENT_ID                                            = var.google_client_id
    GOOGLE_CLIENT_SECRET                                        = var.google_client_secret
    GOOGLE_PHOTO_REFRESH_TOKEN                                  = var.google_photo_refresh_token
    GOOGLE_ALBUM_ID                                             = var.google_album_id
  }
}

resource "azurerm_app_service" "app_service" {
  name                    = "${local.name}-api"
  location                = var.infrastructure_resource_group_location
  resource_group_name     = var.infrastructure_resource_group_name
  app_service_plan_id     = "${data.azurerm_subscription.current.id}/resourceGroups/${var.infrastructure_resource_group_name}/providers/Microsoft.Web/serverfarms/${var.infrastructure_app_service_plan_name}"
  client_affinity_enabled = false
  https_only              = true
  app_settings            = local.api_app_settings

  site_config {
    always_on = true
  }

  tags = var.tags
}

