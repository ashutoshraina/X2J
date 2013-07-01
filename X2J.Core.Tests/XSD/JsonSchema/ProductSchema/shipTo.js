{
  "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/shipTo#",
  "Title": "shipTo",
  "Type": "None",
  "PositionalItemsValidation": false,
  "AllowAdditionalItems": true,
  "UniqueItems": false,
  "Properties": {
    "country": {
      "Type": "String",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true
    },
    "name": {
      "Title": "name",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Extends": [
        {
          "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/name#",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        }
      ]
    },
    "street": {
      "Title": "street",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Extends": [
        {
          "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/street#",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        }
      ]
    },
    "city": {
      "Title": "city",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Extends": [
        {
          "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/city#",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        }
      ]
    },
    "state": {
      "Title": "state",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Extends": [
        {
          "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/state#",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        }
      ]
    },
    "zip": {
      "Title": "zip",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Extends": [
        {
          "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/zip#",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        }
      ]
    }
  },
  "AllowAdditionalProperties": true,
  "Default": null
}