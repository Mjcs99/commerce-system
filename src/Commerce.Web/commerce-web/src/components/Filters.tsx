import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./Filters.css";
export default function Filters(){
    const [openCategories, setOpenCategories] = useState(false);
    const categories = [
    { label: "Caps", to: "/?cat=caps" },
    { label: "Tees", to: "/?cat=tees" },
    { label: "Hoodies", to: "/?cat=hoodies" },
    { label: "Pants", to: "/?cat=pants"}]
    const brands = [
    { label: "Nike", to: "/?brand=Nike" },
    { label: "Adidas", to: "/?brand=Adidas" },
    { label: "UnderArmor", to: "/?brand=UnderArmor" },
    { label: "Reebok", to: "/?brand=Reebok"}]
    return (
  <div className="filters-container">
    <div 
        className="filter-type"
        onClick={() => setOpenCategories(v => !v)}>
        Categories

      {openCategories && (
        <div className="filter-options">
          {categories.map((c) => (
            <Link key={c.label} to={`?cat=${c.label}`}>
              {c.label}
            </Link>
          ))}
        </div>
      )}
    </div>
    <div className="filter-type">Brands</div>
    <div className="filter-type">Materials</div>
    <div className="filter-type">Price</div>
  </div>
);

}