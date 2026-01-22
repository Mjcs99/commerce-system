import { useEffect, useRef, useState } from "react";
import { Link } from "react-router-dom";
import "./Navbar.css";
type NavbarProps = {
  categories: { label: string; to: string }[];
  brands: { label: string; to: string }[];
};

export function Navbar({ categories, brands }: NavbarProps) {
  const [open, setOpen] = useState(false);
  const shopRef = useRef<HTMLDivElement | null>(null);
  const [warning, setWarning] = useState(true);
  // close dropdown when clicking outside
  useEffect(() => {
    function onDocMouseDown(e: MouseEvent) {
      if (!shopRef.current) return;
      if (!shopRef.current.contains(e.target as Node)) setOpen(false);
    }
    document.addEventListener("mousedown", onDocMouseDown);
    return () => document.removeEventListener("mousedown", onDocMouseDown);
  }, []);

  // close dropdown on Escape
  useEffect(() => {
    function onKeyDown(e: KeyboardEvent) {
      if (e.key === "Escape") setOpen(false);
    }
    document.addEventListener("keydown", onKeyDown);
    return () => document.removeEventListener("keydown", onKeyDown);
  }, []);

  return (
    <header className="nav">
        {warning && (<div className="nav-top">
            <h5>
                Due to a temporary supply shortage, some orders may experience a slight
                processing delay. We appreciate your patience.
            </h5>
            <button className="dismissBtn"
                    onClick={() => setWarning(false)}>
                <span>X</span>
            </button>
        </div>
        )}
        <div className="nav-middle">
            <Link className="brand" to="/">
                UrbanStreetwear
            </Link>
            <nav className="middle-links">
                <Link className="navLink" to="/about">
                    About Us
                </Link>
                <Link className="navLink" to="/account">
                    My account
                </Link>
                <Link className="navLink" to="/wishlist">
                    Wishlist
                </Link>
                <Link className="navLink" to="/tracking">
                    Order Tracking
                </Link>
            </nav>
            <div className="nav-middle-info">
  <span>
    <strong>100%</strong>
    <span className="accent">Secure</span>
    <span>transactions with diverse payment options</span>
  </span>

  <span>
    <span>Need help?</span>
    <span className="muted">Call Us:</span>
    <span className="accent">+403 500-8888</span>
  </span>

  <span>
    <span className="currency">CAD</span>
    <span>▼</span>
  </span>
</div>
        </div>

        <div className="nav-bottom">
            <nav className="links">
                <div className="dropdown" ref={shopRef}>
                    <button
                        type="button"
                        className="navBtn"
                        aria-haspopup="menu"
                        aria-expanded={open}
                        onClick={() => setOpen((v) => !v)}
                    >
                        Shop{" "}
                        <span className={`chev ${open ? "chevOpen" : ""}`}>▾</span>
                    </button>

                    {open && (
                        <div className="menu" role="menu">
                            <div className="menu-categories">
                            Categories
                            {categories.map((c) => (
                                <Link
                                    key={c.to}
                                    className="menuItem"
                                    to={c.to}
                                    role="menuitem"
                                    onClick={() => setOpen(false)}
                                >
                                    {c.label}
                                </Link>
                            ))}
                            </div>
                            <div className="menu-brands">
                                Brands
                                {brands.map((c) => (
                                <Link
                                    key={c.to}
                                    className="menuItem"
                                    to={c.to}
                                    role="menuitem"
                                    onClick={() => setOpen(false)}
                                >
                                    {c.label}
                                </Link>
                            ))}
                            </div>
                        </div>
                    )}
                </div>

            </nav>
        </div>
    </header>
);
}
