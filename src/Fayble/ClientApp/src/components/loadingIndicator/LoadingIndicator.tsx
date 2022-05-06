import React from "react";
import { Spinner } from "react-bootstrap";
import styles from "./LoadingIndicator.module.scss";

export const LoadingIndicator = () => {	
	return <Spinner className={styles.spinner} animation="border" />;
};
