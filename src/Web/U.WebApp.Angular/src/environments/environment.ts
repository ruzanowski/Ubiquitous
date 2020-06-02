// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  hostIP: 'localhost',
  apiGatewayPort: '4500',
  productServicePort: '5000',
  smartStoreAdapterPort: '5100',
  fetchServicePort: '5200',
  subscriptionServicePort: '5300',
  identityServicePort: '5400',
  notificationServicePort: '5500',
  signalrPort: '5500',
  signalrEndpoint: 'signalr',
  toastrEnabled: true
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
