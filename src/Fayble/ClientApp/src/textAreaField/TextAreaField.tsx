import cn from "classnames";
import React from "react";
import { Form } from "react-bootstrap";

interface TextAreaFieldProps {
	name: string;
	label: string;
	className?: string;
	rows: number;
	error?: string | false | undefined;
	value?: string;
	onChange: (event: React.ChangeEvent<unknown>) => void;
}

export const TextAreaField: React.FC<TextAreaFieldProps> = ({
	name,
	label,
	className,
	rows,
	error,
	value,
	onChange,
}) => {
	return (
		<Form.Group className={cn(className, "mb-3")}>
			<Form.Label>{label}</Form.Label>
			<Form.Control
				name={name}
				rows={rows}
				as="textarea"
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
