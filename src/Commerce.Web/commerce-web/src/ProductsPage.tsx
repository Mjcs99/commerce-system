import ProductGrid from "./productspage-components/ProductGrid";
import ProductMenu from "./productspage-components/ProductMenu.tsx";
import "./ProductsPage.css"
export default function ProductsPage(){
    return (
    <div className="products-page-container">
        <div className="menu-grid-container">
            <ProductMenu />
            <ProductGrid />
        </div>
    </div>);
}