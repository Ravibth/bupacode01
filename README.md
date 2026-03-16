# E-Commerce Checkout System

A simplified e-commerce checkout system with backend API and React frontend.

## Architecture

- **Backend**: C# .NET Web API with clean architecture (Controller -> Service -> Data)
- **Frontend**: React with routing and context for state management

## Features

- Product listing
- Add items to cart
- Update cart quantities
- Apply coupons (FLAT50, SAVE10)
- Checkout with stock validation
- Order confirmation

## Business Rules

- Cart quantity > 0
- Stock validation on add and checkout
- Coupons:
  - FLAT50: flat $50 discount if subtotal >= $500
  - SAVE10: 10% discount up to $200 max if subtotal >= $1000
- Atomic checkout: stock reduction and order creation

## Setup

### Backend

1. Navigate to `ECommerceAPI/`
2. Run `dotnet build`
3. Run `dotnet run`
4. API available at `http://localhost:5093`
5. Swagger at `http://localhost:5093/swagger`

### Frontend

1. Navigate to `ecommerce-ui/`
2. Run `npm install`
3. Run `npm start`
4. App available at `http://localhost:3000`

## Sample Data

Products:
- Laptop: $50000, Stock: 10
- Mobile: $20000, Stock: 15
- Headphones: $2000, Stock: 20

Coupons: FLAT50, SAVE10

## API Endpoints

- GET /api/products
- POST /api/cart/items
- GET /api/cart/{cartId}
- POST /api/cart/{cartId}/apply-coupon
- POST /api/cart/{cartId}/checkout
- GET /api/orders/{orderId}

## Testing

### Backend (.NET)

- **Run all backend tests**: from `ECommerceAPI/` run:
  - `dotnet test`
- **Current unit tests cover**:
  - Coupon validation (valid and invalid FLAT50 scenarios)
  - Checkout pricing (subtotal, discount, grand total)
  - Stock error scenario on checkout (insufficient stock)

### Frontend (React)

- From `ecommerce-ui/` you can run:
  - `npm test`
  - This uses React Testing Library / Jest for UI tests.