################################################################################
# General
################################################################################

aws_region = "eu-central-1"
az_letter  = "a"
tags = {
  owner = "terraform"
}

################################################################################
# VPC variables
################################################################################

vpc_cidr = "10.0.0.0/16"
vpc_name = "test"
subnets = {
  subnet0 = {
    cidr_block = "10.0.1.0/24"
    public     = true
    az_index   = 0
  },
  subnet1 = {
    cidr_block = "10.0.2.0/24"
    public     = true
    az_index   = 1
  },
  subnet2 = {
    cidr_block = "10.0.3.0/24"
    public     = false
    az_index   = 0
  }
  subnet3 = {
    cidr_block = "10.0.4.0/24"
    public     = false
    az_index   = 1
  }

}

###############################################
# Security Groups variables and ALB variables
###############################################

name_prefix       = "app"
project_name      = "app"
alb_ingress_cidr  = ["0.0.0.0/0"]
alb_ingress_ports = [80, 8080]
web_backend_port  = 8080
web_ui_port       = 80

################################################################################
# EC2 configuration
################################################################################
#Amazon Linux 2023 AMI 2023.8.20250808.1 x86_64 HVM kernel-6.1 in eu-central-1

ami           = "ami-015cbce10f839bd0c"
ec2_name_set  = ["react", "angular", "dotnet"]
subnet        = ""
sgs           = []
instance_type = "t3.micro"
iam_role_policies = {
  AdministratorAccess = "arn:aws:iam::aws:policy/AdministratorAccess",
  SSM                 = "arn:aws:iam::aws:policy/AmazonSSMManagedInstanceCore"
}

################################################################################
# RDS variables
################################################################################

rds_port             = 5432
db_id                = "postgres-db"
db_username          = "postgres"
db_engine            = "postgres"
db_engine_version    = "17.5"
db_instance_class    = "db.t3.micro"
db_subnet_group_name = "rds-private-subnet-group"

################################################################################
# SSL variables
################################################################################

domain_name = "fenvis.pp.ua"
certificate_arn = "arn:aws:acm:eu-central-1:297486149860:certificate/ed86fab1-f2df-4d26-873f-de060b4f4c22"
