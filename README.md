
**.NET 8 API for restaurants - Made for waiters and bartenders so orders can be placed, served and processed faster and easier**

I decided to build this project because I saw a real-world need for such systems as more and more restaurants are using them.

This is a RESTful API built with .NET 8, MySQL, uses JWTs for user authentication and authorization, real time notifications  with a controller-service-repository structure.

It has features like real email code confirmation, websockets, calculating order total price and stock checking, admin users, token refresh etc.



## Run Locally 

Docker compose has neeeded env variables so you can run it. Configured to run on port 5000 and MySQL on 3307.

**Run following commands**   
   ```bash
   git clone https://github.com/BakirMehremic/DrinkConnect
   cd DrinkConnect
   docker compose up --build
   http://localhost:5000/swagger/index.html
```

## Endpoints

### Authentication (Admin)

| Method | Endpoint          | Description                     |
|--------|-------------------|---------------------------------|
| POST   | `/register`       | Register a new user (Admin only)|
| POST   | `/confirmemail`   | Confirm user email              |
| POST   | `/login`          | Login and obtain JWT token      |
| POST   | `/refresh-token`  | Refresh JWT token               |

### Bartender

| Method | Endpoint                              | Description                     |
|--------|---------------------------------------|---------------------------------|
| POST   | `/bartender/product`                 | Add a new product               |
| PUT    | `/bartender/product/{id}`            | Edit an existing product        |
| DELETE | `/bartender/product/{id}`            | Delete a product                |
| GET    | `/bartender/product/{id}`            | Get product details             |
| PUT    | `/bartender/order/{id}/status`       | Update order status             |
| GET    | `/bartender/orders`                  | Get all orders                  |
| GET    | `/bartender/order/{id}`              | Get specific order details      |
| DELETE | `/bartender/order/{id}`              | Delete an order                 |
| GET    | `/bartender/notification/{id}`       | Get notification details        |
| DELETE | `/bartender/notification/{id}`       | Delete a notification           |

### Waiter

| Method | Endpoint                      | Description                     |
|--------|-------------------------------|---------------------------------|
| POST   | `/waiter/order`               | Place a new order               |
| PUT    | `/waiter/order/{id}`          | Edit an order                   |
| DELETE | `/waiter/order/{id}`          | Delete an order                 |
| GET    | `/waiter/order/{id}`          | Get specific order details      |
| GET    | `/waiter/orders`              | Get all orders                  |

### WebSocket

| Protocol | Endpoint                              | Description                     |
|----------|---------------------------------------|---------------------------------|
| WS       | `ws://localhost:5037/notifications`  | WebSocket endpoint for real-time notifications |
