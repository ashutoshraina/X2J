{
  "Id": "http://tempuri.org/PurchaseOrderSchema.xsd/items#",
  "Title": "items",
  "Type": "None",
  "PositionalItemsValidation": false,
  "AllowAdditionalItems": true,
  "UniqueItems": false,
  "Properties": {
    "item": {
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
  },
  "AllowAdditionalProperties": true,
  "Default": null
}