import React, { useState, useEffect } from 'react';
import { useCart } from './CartContext';
import { API_BASE } from './Constants';

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [notification, setNotification] = useState('');
  const { addToCart } = useCart();

  useEffect(() => {
    fetch(`${API_BASE}/products`)
      .then(res => res.json())
      .then(data => {
        setProducts(data);
        setLoading(false);
      });
  }, []);

  const handleAddToCart = async (productId) => {
    const success = await addToCart(productId, 1);
    if (success) {
      setNotification('Added to cart!');
      setTimeout(() => setNotification(''), 2500);
    }
  };

  if (loading) return <div>Loading products...</div>;

  return (
    <div>
      <h2>Products</h2>
      {notification && <div className="notification">{notification}</div>}
      <div className="product-grid">
        {products.map(product => (
          <div key={product.id} className="card product-card">
            <div className="product-image">
              <img
                src={product.imageUrl || 'https://via.placeholder.com/220x160.png?text=Product'}
                alt={product.name}
              />
            </div>
            <div className="product-meta">
              <div>
                <h3>{product.name}</h3>
                {product.description && (
                  <p style={{ margin: '6px 0 10px 0', color: '#475569', fontSize: '0.93rem' }}>
                    {product.description}
                  </p>
                )}
                <p style={{ margin: '8px 0 4px 0' }}>Price: <strong>${product.price}</strong></p>
                <p style={{ margin: 0, color: product.stock > 0 ? '#2563eb' : '#dc2626' }}>
                  {product.stock > 0 ? `In stock: ${product.stock}` : 'Out of stock'}
                </p>
              </div>
              <button
                className="button button_primary"
                onClick={() => handleAddToCart(product.id)}
                disabled={product.stock <= 0}
              >
                Add to Cart
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ProductList;