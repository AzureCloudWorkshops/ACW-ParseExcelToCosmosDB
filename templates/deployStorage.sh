rg=parseexceltocosmos
loc=centralus

#create/ensure resource group
az group create -n $rg -l $loc

#create storage acount and container for uploads
az deployment group create --resource-group $rg --template-file azuredeploystorage.bicep
