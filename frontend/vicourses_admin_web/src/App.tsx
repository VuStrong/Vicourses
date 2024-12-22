import { useEffect } from "react";
import { useLocation } from "react-router-dom";
import AppRoutes from "./routes";
import useUser from "./hooks/useUser";

function App() {
    const { pathname } = useLocation();
    const initialize = useUser(state => state.initialize);

    useEffect(() => {
        window.scrollTo(0, 0);
    }, [pathname]);

    useEffect(() => {
        initialize();
    }, []);

    return <AppRoutes />;
}

export default App;
