# CyberArk.Extensions.ServiceNow
CyberArk CPM Plugin that supports management on local ServiceNow Credentials using the ServiceNow REST API.

## Supported CPM Actions
- [x] Verify
- [x] Change
- [x] Reconcile

## Overview
This plugin interacts with the ServiceNow sys_user table to rotate and manage user credentials. Each process is covered at a high level below:

### Verify
The verify process utilizes the target account credentials to authenticate to the ServiceNow REST API and performs a query against the sys_user table and return the target account sys_id.
```
GET https://{{tenant}}.service-now.com/api/now/table/sys_user?sysparm_display_value=true&sysparm_query=user_name={{target_account}}&sysparm_fields=sys_id&sysparm_limit=1
```
Basic read permissions on the sys_user table are required for this API call. As long as the users sys_id is returned the verify action completes succesfully.

### Change
The change process utilizes the target account credentials to authenticate to the ServiceNow REST API and performs a query against the sys_user table and return the target account sys_id.
```
GET https://{{tenant}}.service-now.com/api/now/table/sys_user?sysparm_display_value=true&sysparm_query=user_name={{target_account}}&sysparm_fields=sys_id&sysparm_limit=1
```
After retrieving the users sys_id, an update operation is perfomred against the sys_user table using the target accounts current credentials.
```
PATCH https://{{tenant}}.service-now.com/api/now/table/sys_user/{{target_account_sys_id}}?sysparm_input_display_value=truesysparm_fields=user_password
```
The Target Account requires write permissions on the sys_user table for the target account are required for this API call. This patch operation should return the hashed password in order for the change action to complete successfully. 

### Reconcile
The reconcile process utilizes the reconcile account credentials to authenticate to the ServiceNow REST API and performs a query against the sys_user table and return the target account sys_id.
```
GET https://{{tenant}}.service-now.com/api/now/table/sys_user?sysparm_display_value=true&sysparm_query=user_name={{target_account}}&sysparm_fields=sys_id&sysparm_limit=1
```
After retrieving the users sys_id, an update operation is perfomred against the sys_user table using the target accounts current credentials.
```
PATCH https://{{tenant}}.service-now.com/api/now/table/sys_user/{{target_account_sys_id}}?sysparm_input_display_value=truesysparm_fields=user_password
```
The Reconcile Account requires write permissions on the sys_user table for the target account are required for this API call. This patch operation should return the hashed password in order for the change action to complete successfully. 




