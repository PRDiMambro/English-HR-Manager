# English-HR-Manager
This portal system aims to integrate English Human Resources and Payroll Systems.


## Bank Holidays Function
This function gets the official United Kingdom bank holidays from a [gov.uk json file](https://www.gov.uk/bank-holidays.json) and delivers the *England and Wales* dates to an Azure blob storage *csv file* both in *descending order* and filtered by url *selected year*.


#### 1. Bank Holidays Function - Trigger
It's *HTTP triggered*, so it'll start when the following url is used:

**[https://english-hr-manager.azurewebsites.net/api/bank-holidays/$year](https://english-hr-manager.azurewebsites.net/api/bank-holidays/2021)**
  
Where you need to replace the *$year* substring above by the desired year or it will return nothing.

  
#### 2. Bank Holidays Function - Outputs
The Function delivers the dates two ways:

• For easier inspection, it outputs the csv formatted dates once triggered, so that if you open the url on a browser they'll be shown on the display window as a confirmation the function processed adequately.
  
• A [BankHolidays.csv](https://englishhrmanager.blob.core.windows.net/bankholidays/BankHolidays.csv) file is delivered an Azure blob storage container, so it can be used later by other functions/apps.
