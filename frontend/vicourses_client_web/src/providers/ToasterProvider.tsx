"use client";

import { Toaster } from "react-hot-toast";

const ToasterProvider = () => {
    return <Toaster containerStyle={{ zIndex: 99999 }} />;
};

export default ToasterProvider;
