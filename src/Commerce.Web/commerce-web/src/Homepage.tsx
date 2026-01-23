import BestSellers from "./components/BestSellers";
import Filters from "./components/Filters";
import "./Homepage.css";
export default function Homepage(){
    return(
        <div className="homepage-layout">
            <Filters />
            <BestSellers />
        </div>
        
    )
}