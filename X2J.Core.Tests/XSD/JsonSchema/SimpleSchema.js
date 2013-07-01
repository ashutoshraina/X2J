{
  "Id": "http://tempuri.org/PurchaseOrderSchema.xsd",
  "Title": "SimpleSchema",
  "PositionalItemsValidation": false,
  "AllowAdditionalItems": true,
  "UniqueItems": false,
  "Properties": {
    "PurchaseOrderType": {
      "Type": "String",
      "Items": [
        {
          "Title": "ShipTo",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/ShipTo#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        },
        {
          "Title": "BillTo",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/BillTo#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        }
      ],
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "Properties": {},
      "AllowAdditionalProperties": true
    },
    "USAddress": {
      "Type": "String",
      "Items": [
        {
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
        {
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
        {
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
        {
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
        {
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
      ],
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "Properties": {},
      "AllowAdditionalProperties": true
    },
    "PurchaseOrder": {
      "Type": "None",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "Properties": {
        "OrderDate": {
          "Type": "String",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Format": "Date"
        },
        "ShipTo": {
          "Title": "ShipTo",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/ShipTo#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        },
        "BillTo": {
          "Title": "BillTo",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/BillTo#",
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
  },
  "AllowAdditionalProperties": true
}