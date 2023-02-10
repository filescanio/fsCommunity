The fsutil utility is used to provide sample code on accessing the OPSWAT Filescan server


To get the latest commands that are available run 

"fsutil help" 

This tool requires an APIKEY.  If you do not already have an API Key you can generate one by following the instructions found at:
https://www.filescan.io/users/profile

Once a key is created you can test uploading a file with the following command:
fsutil scanfile --apikey %APIKEY% --input %FileToScan%

To pull the results use the following command:
fsutil results --apikey %APIKEY% --output %csvFile%



This tool should also work for a stand-alone instance of filescan.
Modify serverConfig.json with the correct serverURL.


Compiling the project should be easy if you download Microsoft Visaul Studio Community Edition

Open the project src\fsutil.sln
The project should compile