import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import { CartProvider, useCart } from './CartContext';
import ProductList from './ProductList';
import Cart from './Cart';
import './App.css';

const HeaderNav = () => {
  const { cart } = useCart();
  const itemCount = cart?.items?.reduce((sum, item) => sum + item.quantity, 0) || 0;

  return (
    <nav>
      <Link to="/">Products</Link> | <Link to="/cart">Cart ({itemCount})</Link>
    </nav>
  );
};

function App() {
  return (
    <CartProvider>
      <Router>
        <div className="App">
          <header>
            <h1>E-Commerce Store</h1>
            <HeaderNav />
          </header>
          <main>
            <Routes>
              <Route path="/" element={<ProductList />} />
              <Route path="/cart" element={<Cart />} />
            </Routes>
          </main>
        </div>
      </Router>
    </CartProvider>
  );
}

export default App;
