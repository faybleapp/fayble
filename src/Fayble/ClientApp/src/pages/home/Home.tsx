import React from "react";
import { useLibraries } from "services/library";

export const Home = () => {
    const { isLoading, data } = useLibraries();
    console.log(data);

    return (
        <></>
    );
};
