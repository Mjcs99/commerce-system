import "./ProductGrid.css";
import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { useLocation } from "react-router-dom";
import { getProducts } from "./Api/ProductsApiClient.ts";
import type { Product } from "../types/Product.ts";

export default function ProductGrid(){
    const location = useLocation();
    const params = new URLSearchParams(location.search);
    const [products, setProducts] = useState<Product[]>([]);

    useEffect(() => {
      async function loadProducts() {
        const products = (await getProducts(params.toString() ? params : undefined)).items;
        setProducts(products);
    }
    loadProducts();
    }, [location.search]);

    return (
    <div className="grid-container">
        {products.map(p => 
            <div className="product-card">
                <img src={p.primaryImageUrl} />
                <p>{p.name}</p>
                <p>{p.priceAmount}</p>
            </div>
        )}
    </div>);
}