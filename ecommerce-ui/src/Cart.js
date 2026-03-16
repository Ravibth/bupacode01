import React, { useState, useEffect } from 'react';
import { useCart } from './CartContext';
import { useNavigate } from 'react-router-dom';
import { API_BASE } from './Constants';

const Cart = () => {
  const { cart, loading, error, fetchCart, addToCart, removeFromCart, applyCoupon, checkout, setError } = useCart();
  const [couponCode, setCouponCode] = useState('');
  const [order, setOrder] = useState(null);
  const [productMap, setProductMap] = useState({});
  const [selected, setSelected] = useState({});
  const navigate = useNavigate();

  useEffect(() => {
    fetchCart();
    fetchProducts();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (!cart?.items) return;
    const allSelected = {};
    cart.items.forEach(item => {
      allSelected[item.productId] = true;
    });
    setSelected(allSelected);
  }, [cart]);

  const fetchProducts = async () => {
    const response = await fetch(`${API_BASE}/products`);
    const products = await response.json();
    const map = products.reduce((acc, p) => ({ ...acc, [p.id]: p }), {});
    setProductMap(map);
  };

  const handleQuantityChange = (productId, quantity) => {
    if (quantity > 0) {
      addToCart(productId, quantity);
    } else {
      removeFromCart(productId);
    }
  };

  const toggleSelect = (productId) => {
    setSelected(prev => ({ ...prev, [productId]: !prev[productId] }));
  };

  const toggleSelectAll = () => {
    const allSelected = cart?.items?.every(item => selected[item.productId]);
    const next = {};
    (cart?.items || []).forEach(item => {
      next[item.productId] = !allSelected;
    });
    setSelected(next);
  };

  const selectedItems = cart?.items?.filter(item => selected[item.productId]) || [];
  const selectedCount = selectedItems.length;
  const selectedQuantity = selectedItems.reduce((sum, item) => sum + item.quantity, 0);
  const selectedSubtotal = selectedItems.reduce((sum, item) => sum + item.price * item.quantity, 0);

  const handleApplyCoupon = () => {
    applyCoupon(couponCode);
  };

  const handleCheckout = async () => {
    const result = await checkout();
    if (result) {
      setOrder(result);
    }
  };

  if (order) {
    return (
      <div>
        <h2>Order Confirmation</h2>
        <p>Order ID: {order.orderId}</p>
        <p>Subtotal: ${order.subtotal}</p>
        <p>Discount: ${order.discount}</p>
        <p>Tax: ${order.tax}</p>
        <p>Grand Total: ${order.grandTotal}</p>
        <h3>Items:</h3>
        <ul>
          {order.items.map(item => (
            <li key={item.productId}>{item.productId} - Qty: {item.quantity} - ${item.price * item.quantity}</li>
          ))}
        </ul>
        <button className="button button_secondary" onClick={() => navigate('/')}>Back to Products</button>
      </div>
    );
  }

  if (loading) return <div>Loading cart...</div>;
  if (error) return (
    <div style={{ marginBottom: 16 }}>
      <div style={{ padding: 12, background: '#ffe4e6', border: '1px solid #fca5a5', borderRadius: 10 }}>
        {error}
      </div>
      <button className="button button_secondary" onClick={() => setError(null)}>Dismiss</button>
    </div>
  );

  const totalSubtotal = cart?.items?.reduce((sum, item) => sum + item.price * item.quantity, 0) || 0;

  return (
    <div>
      <div style={{ display: 'flex', alignItems: 'baseline', justifyContent: 'space-between', gap: 12 }}>
        <h2>Shopping Cart</h2>
        {cart?.items?.length > 0 && (
          <button className="button button_secondary" onClick={toggleSelectAll}>
            {selectedCount === (cart.items?.length || 0) ? 'Unselect all' : 'Select all items'}
          </button>
        )}
      </div>
      {cart?.items?.length > 0 && (
        <div style={{ marginTop: 6, color: '#475569', fontSize: '0.95rem' }}>
          {selectedCount} item{selectedCount === 1 ? '' : 's'} selected (qty: {selectedQuantity})
        </div>
      )}
      {cart?.items?.length === 0 ? (
        <p>Your cart is empty.</p>
      ) : (
        <>
          <div className="cart-list">
            {cart.items.map(item => {
              const product = productMap[item.productId] || {};
              const imageUrl = product.imageUrl || 'https://via.placeholder.com/140x110.png?text=Item';
              return (
                <div key={item.productId} className="cart-item">
                  <div className="cart-item-selection">
                    <input
                      type="checkbox"
                      checked={!!selected[item.productId]}
                      onChange={() => toggleSelect(item.productId)}
                    />
                  </div>
                  <div className="cart-item-media">
                    <img src={imageUrl} alt={product.name || 'Item'} />
                  </div>
                  <div className="cart-item-content">
                    <div className="cart-item-header">
                      <div>
                        <div className="product-title">{product.name || `Product ${item.productId}`}</div>
                        <div className="product-meta">{product.description || ''}</div>
                        <div className="product-stock" style={{ color: product.stock > 0 ? '#059669' : '#b91c1c' }}>
                          {product.stock > 0 ? `In stock (${product.stock})` : 'Out of stock'}
                        </div>
                      </div>
                      <div className="cart-item-price">
                        <div className="price">₹{item.price.toFixed(2)}</div>
                        <div className="price-sub">₹{(item.price * item.quantity).toFixed(2)} total</div>
                      </div>
                    </div>

                    <div className="cart-item-actions">
                      <div className="qty-control">
                        <button
                          className="button button_secondary"
                          onClick={() => handleQuantityChange(item.productId, Math.max(0, item.quantity - 1))}
                        >
                          -
                        </button>
                        <input
                          type="number"
                          value={item.quantity}
                          onChange={(e) => handleQuantityChange(item.productId, parseInt(e.target.value) || 0)}
                          min="0"
                        />
                        <button
                          className="button button_secondary"
                          onClick={() => handleQuantityChange(item.productId, item.quantity + 1)}
                        >
                          +
                        </button>
                      </div>
                      <div className="item-buttons">
                        <button className="button button_secondary" onClick={() => removeFromCart(item.productId)}>
                          Delete
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              );
            })}
          </div>

          <div className="cart-summary">
            <div className="summary-box">
              <p>Selected items: <span>{selectedQuantity}</span></p>
              <p>Subtotal (selected): <span>₹{selectedSubtotal.toFixed(2)}</span></p>
              <p>Discount: <span>₹{(cart.discount || 0).toFixed(2)}</span></p>
              <p>Total: <span>₹{((cart.totalAmount || totalSubtotal)).toFixed(2)}</span></p>
            </div>

            <div className="summary-box">
              <div style={{ marginBottom: 8 }}>
                <input
                  type="text"
                  placeholder="Coupon code"
                  value={couponCode}
                  onChange={(e) => setCouponCode(e.target.value)}
                  style={{ width: '100%', padding: '8px 10px', borderRadius: 10, border: '1px solid rgba(0,0,0,0.15)' }}
                />
              </div>
              <button className="button button_primary" onClick={handleApplyCoupon}>Apply Coupon</button>
              <div style={{ marginTop: 12 }}>
                <button className="button button_primary" onClick={handleCheckout}>Checkout</button>
              </div>
            </div>
          </div>
        </>
      )}

      <button className="button button_secondary" onClick={() => navigate('/')}>Back to Products</button>
    </div>
  );
};

export default Cart;