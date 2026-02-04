# Trados UI extensibility

Functionality for [Trados](https://www.trados.com/) user interface extensibility.

## Usage

```javascript
import {
  trados,
  ExtensionElement,
  ExtensibilityEventDetail
} from "@trados/trados-ui-extensibility";

// define custom elements
const elements: ExtensionElement[] = [
  {
    elementId: "myCustomButton",
    location: "project-details-toolbar",
    text: "My custom button",
    type: "button",
    actions: [
      {
        eventType: "onclick",
        eventHandler: (detail: ExtensibilityEventDetail) => {
          console.log("My custom button was clicked", detail.project);
        },
        payload: ["project"]
      }
    ]
  }
];

// register extension with Trados
trados.onReady(elements, () => {
  console.log("Trados UI extension registered");
});
```

## License

The functionality is available with a [MIT license](https://choosealicense.com/licenses/mit/).
