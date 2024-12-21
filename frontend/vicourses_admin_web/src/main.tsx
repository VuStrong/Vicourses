import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import "./css/style.css";
import "./css/satoshi.css";
import "flatpickr/dist/flatpickr.min.css";
import { Toaster } from "react-hot-toast";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
    <React.StrictMode>
        <BrowserRouter>
            <Toaster containerStyle={{ zIndex: 100001 }} />
            <App />
        </BrowserRouter>
    </React.StrictMode>,
);
