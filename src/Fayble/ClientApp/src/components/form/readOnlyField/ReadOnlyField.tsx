import cn from "classnames";
import React from "react";
import { Form } from "react-bootstrap";
import styles from "./ReadOnlyField.module.scss";

interface ReadOnlyFieldProps {
	value?: string;
	className?: string;
	label: string;
}

export const ReadOnlyField = ({ value, className, label }: ReadOnlyFieldProps) => {
	return (
		<Form.Group className={cn(className, "mb-3")}>
			<Form.Label>{label}</Form.Label>
			<Form.Control value={value} readOnly plaintext className={styles.readOnlyField} />
		</Form.Group>
	);
};
