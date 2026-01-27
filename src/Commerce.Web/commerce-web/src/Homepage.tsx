import BestSellers from "./homepage-componenets/BestSellers";
import HeaderAdvertisement from "./homepage-componenets/HeaderAdvertisement";
import "./Homepage.css";
export default function Homepage(){
    return(
        <div className="homepage-container">
            <HeaderAdvertisement />
            <div className="section-divider"></div>
            <div className="homepage-layout">
                <BestSellers />
            </div>
        </div>
    )
}