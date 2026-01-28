import BestSellers from "../home-page-components/BestSellers";
import HeaderAdvertisement from "../home-page-components/HeaderAdvertisement";
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