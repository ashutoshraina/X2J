{
  "Title": "Relationships",
  "PositionalItemsValidation": false,
  "AllowAdditionalItems": true,
  "UniqueItems": false,
  "Properties": {
    "NameAgeAttributes": {
      "Description": "",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "Properties": {
        "age": {
          "Required": true,
          "Type": "Integer",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        },
        "name": {
          "Required": true,
          "Type": "String",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true
        }
      },
      "AllowAdditionalProperties": true
    },
    "PetType": {
      "Type": "String",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "Properties": {},
      "AllowAdditionalProperties": true
    },
    "pet": {
      "Type": "None",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "Properties": {},
      "AllowAdditionalProperties": true,
      "Default": null
    },
    "cat": {
      "Type": "None",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "Properties": {
        "weight": {
          "Title": "weight",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "/weight#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        },
        "color": {
          "Title": "color",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "/color#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        },
        "breed": {
          "Title": "breed",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "/breed#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        }
      },
      "AdditionalProperties": {
        "PositionalItemsValidation": false,
        "AllowAdditionalItems": true,
        "UniqueItems": false,
        "AllowAdditionalProperties": true,
        "Extends": [
          {
            "Id": "/pet#",
            "PositionalItemsValidation": false,
            "AllowAdditionalItems": true,
            "UniqueItems": false,
            "AllowAdditionalProperties": true
          }
        ]
      },
      "AllowAdditionalProperties": true,
      "Default": null
    },
    "dog": {
      "Type": "None",
      "PositionalItemsValidation": false,
      "AllowAdditionalItems": true,
      "UniqueItems": false,
      "Properties": {
        "weight": {
          "Title": "weight",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "/weight#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        },
        "color": {
          "Title": "color",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "/color#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        },
        "breed": {
          "Title": "breed",
          "PositionalItemsValidation": false,
          "AllowAdditionalItems": true,
          "UniqueItems": false,
          "AllowAdditionalProperties": true,
          "Extends": [
            {
              "Id": "/breed#",
              "PositionalItemsValidation": false,
              "AllowAdditionalItems": true,
              "UniqueItems": false,
              "AllowAdditionalProperties": true
            }
          ]
        }
      },
      "AdditionalProperties": {
        "PositionalItemsValidation": false,
        "AllowAdditionalItems": true,
        "UniqueItems": false,
        "AllowAdditionalProperties": true,
        "Extends": [
          {
            "Id": "/pet#",
            "PositionalItemsValidation": false,
            "AllowAdditionalItems": true,
            "UniqueItems": false,
            "AllowAdditionalProperties": true
          }
        ]
      },
      "AllowAdditionalProperties": true,
      "Default": null
    }
  },
  "AllowAdditionalProperties": true
}