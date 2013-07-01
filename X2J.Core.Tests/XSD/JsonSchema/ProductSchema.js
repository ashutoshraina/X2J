{
  "Id": "http://tempuri.org/PurchaseOrderSchema.xsd",
  "Title": "ProductSchema",
  "PositionalItemsValidation": false,
  "AllowAdditionalItems": true,
  "UniqueItems": false,
  "Properties": {
    "SKU": {
      "Type": "Integer, Boolean",
      "Pattern": "\\d{3}\\w{3}",
      "PositionalItemsValidation": false,
      "": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true
    },
    "USAddress": {
      "Description": "Purchase order schema for Example.Microsoft.com.",
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
    "Items": {
      "Type": "String",
      "Items": [
        {
          "Title": "item",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/item#",
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
    "PurchaseOrderType": {
      "Type": "String",
      "Items": [
        {
          "Title": "shipTo",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/shipTo#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        },
        {
          "Title": "billTo",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/billTo#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        },
        {
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
        {
          "Title": "items",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/items#",
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
    "comment": {
      "Type": "Integer, Boolean",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "AllowAdditionalProperties": true,
      "Default": null
    },
    "purchaseOrder": {
      "Type": "None",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "Properties": {
        "orderDate": {
          "Type": "String",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Format": "Date"
        },
        "confirmDate": {
          "Required": true,
          "Type": "String",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Format": "Date"
        },
        "shipTo": {
          "Title": "shipTo",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/shipTo#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        },
        "billTo": {
          "Title": "billTo",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/billTo#",
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
        "items": {
          "Title": "items",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/items#",
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