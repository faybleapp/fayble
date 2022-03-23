import React from "react";
import { Form } from "react-bootstrap";

interface NumberFieldProps {
	name: string;
	label: string;
	className?: string;
	error?: string | false | undefined;
	value?: number;
	onChange: (event: React.ChangeEvent<unknown>) => void;
}

export const NumberField = ({
	name,
	label,
	className,
	error,
	value,
	onChange,
}: NumberFieldProps) => {
	return (
		<Form.Group className={className}>
			<Form.Label>{label}</Form.Label>
			<Form.Control
				name={name}
				type="number"
				isInvalid={!!error}
				value={value}
				onChange={onChange}
			/>
			<Form.Control.Feedback type="invalid">
				{error}
			</Form.Control.Feedback>
		</Form.Group>
	);
};
