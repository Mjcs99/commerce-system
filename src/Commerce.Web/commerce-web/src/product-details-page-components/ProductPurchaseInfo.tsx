import styles from "./ProductPurchaseInfo.module.css";
import type { ProductDetails } from "../types/ProductDetails.ts";
export default function ProductPurchaseInfo({ product }: { product: ProductDetails }) {
    return(
        <div className={styles.purchaseInfoContainer}>
            <h1>{product.name}</h1>
            <p className={styles.price}>$99.99</p>
            <button className={styles.addToCartButton}>Add to Cart</button>
        </div>
    )
}