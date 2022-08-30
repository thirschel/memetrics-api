variable "infrastructure_resource_group_name" {
  description = "The name of the infrastructure resource group"
}

variable "infrastructure_resource_group_location" {
  description = "The location of the infrastructure resource group"
}

variable "infrastructure_app_service_plan_name" {
  description = "App Service Plan ID to use"
}

variable "tags" {
  description = "A mapping of tages to assign to the resource. Changing this forces a new resource to be created."
  default = {
    source  = "terraform"
    product = "MeMetrics"
  }
}

variable "app_insights_intrumentation_key" {}
variable "aspnetcore_environment" {}
variable "env" {}
variable "log_level" {}
variable "docker_api_custom_image_name" {}
variable "docker_registry_server_url" {}
variable "docker_registry_server_username" {}
variable "docker_registry_server_password" {}
variable "db_connection_string" {}
variable "primary_api_key" {}
variable "secondary_api_key" {}
variable "allowed_origin" {}