{
  "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/item#",
  "Title": "item",
  "Type": "None",
  "PositionalItemsValidation": false,
  "AllowAdditionalItems": true,
  "UniqueItems": false,
  "Properties": {
    "partNum": {
      "Type": "String",
      "Pattern": "\\d{3}\\w{3}",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true
    },
    "productName": {
      "Title": "productName",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Extends": [
        {
          "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/productName#",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        }
      ]
    },
    "quantity": {
      "Title": "quantity",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Extends": [
        {
          "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/quantity#",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        }
      ]
    },
    "USPrice": {
      "Title": "USPrice",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Extends": [
        {
          "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/USPrice#",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        }
      ]
    },
    "comment": {
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Extends": [
        {
          "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/comment#",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        }
      ]
    },
    "shipDate": {
      "Title": "shipDate",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Extends": [
        {
          "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/shipDate#",
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