rg=parseexceltocosmos
loc=centralus

#create/ensure resource group
az group create -n $rg -l $loc

#create function app and backing storage for function app
az deployment group create --resource-group $rg --template-file azuredeployfunctionapp.bicep