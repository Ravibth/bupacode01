import React, { createContext, useContext, useState } from 'react';
import { API_BASE } from './Constants';
const CartContext = createContext();

export const useCart = () => useContext(CartContext);

export const CartProvider = ({ children }) => {
  const [cartId] = useState('user-cart'); // Simple cart ID
  const [cart, setCart] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fetchCart = async () => {
    setLoading(true);
    try {
      const response = await fetch(`${API_BASE}/cart/${cartId}`);
      if (response.ok) {
        const data = await response.json();
        setCart(data);
      } else {
        setCart({ id: cartId, items: [], discount: 0, totalAmount: 0 });
      }
    } catch (err) {
      setError(err.message);
    }
    setLoading(false);
  };

  // Keep cart state initialized so we can show counts in the UI immediately.
  React.useEffect(() => {
    fetchCart();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const addToCart = async (productId, quantity) => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetch(`${API_BASE}/cart/items`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ cartId, item: { productId, quantity } })
      });
      if (response.ok) {
        const data = await response.json();
        setCart(data);
        return true;
      } else {
        const text = await response.text();
        setError(text);
        return false;
      }
    } catch (err) {
      setError(err.message);
      return false;
    } finally {
      setLoading(false);
    }
  };

  const removeFromCart = async (productId) => {
    setLoading(true);
    setError(null);

    try {
      const response = await fetch(`${API_BASE}/cart/${cartId}/items/${productId}`, {
        method: 'DELETE'
      });
      if (response.ok) {
        const data = await response.json();
        setCart(data);
        return true;
      } else {
        const text = await response.text();
        setError(text);
        return false;
      }
    } catch (err) {
      setError(err.message);
      return false;
    } finally {
      setLoading(false);
    }
  };

  const applyCoupon = async (code) => {
    setLoading(true);
    try {
      const response = await fetch(`${API_BASE}/cart/${cartId}/apply-coupon`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ code })
      });
      if (response.ok) {
        const data = await response.json();
        setCart(data);
      } else {
        setError(await response.text());
      }
    } catch (err) {
      setError(err.message);
    }
    setLoading(false);
  };

  const checkout = async () => {
    setLoading(true);
    try {
      const response = await fetch(`${API_BASE}/cart/${cartId}/checkout`, {
        method: 'POST'
      });
      if (response.ok) {
        const order = await response.json();
        setCart({ id: cartId, items: [], discount: 0, totalAmount: 0 });
        return order;
      } else {
        setError(await response.text());
        return null;
      }
    } catch (err) {
      setError(err.message);
      return null;
    } finally {
      setLoading(false);
    }
  };

  return (
    <CartContext.Provider value={{
      cart,
      loading,
      error,
      fetchCart,
      addToCart,
      removeFromCart,
      applyCoupon,
      checkout,
      setError
    }}>
      {children}
    </CartContext.Provider>
  );
};