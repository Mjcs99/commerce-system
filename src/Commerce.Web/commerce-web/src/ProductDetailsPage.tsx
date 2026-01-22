import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";

type ProductDetails = {
    productId: string;
    name: string;
    images: string[];
    description: string;
}
export default function ProductDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<ProductDetails | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!id) return;

    async function loadProduct() {
      try {
        setLoading(true);
        const res = await fetch(`/api/v1/products/${id}/details`);
        if (!res.ok) throw new Error(`Failed (${res.status})`);
        setProduct(await res.json());
      } catch (e) {
        setError((e as Error).message);
      } finally {
        setLoading(false);
      }
    }
    loadProduct();
  }, [id]);

  return (
    <div style={{ padding: 24 }}>
      <Link to="/">← Back</Link>
      {loading && <p>Loading…</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}
      {product && (
        <>
          <h1>{product.name}</h1>
          <p>{product.description}</p>
          {product.images && (product.images.map(url => <img src={url} alt={product.name} />))}
        </>
      )}
    </div>
  );
}
