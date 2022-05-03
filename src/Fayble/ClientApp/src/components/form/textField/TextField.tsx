import cn from "classnames";
import React from "react";
import { Form } from "react-bootstrap";

interface TextFieldProps {
	name: string;
	label?: string;
	className?: string;
	error?: string | false | undefined;
	value?: string;
	secure?: boolean;
	placeholder?: string;
	onBlur?: (event: React.FocusEvent<HTMLInputElement>) => void;
	onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export const TextField = ({
	name,
	label,
	className,
	error,
	value,
	secure,
	placeholder,
	onChange,
	onBlur,
}: TextFieldProps) => {
	return (
		<Form.Group className={cn(className, "mb-3")}>
			{label && <Form.Label>{label}</Form.Label>}
			<Form.Control
				name={name}
				type={secure ? "password" : "text"}
				isInvalid={!!error}
				value={value}
				onBlur={onBlur}
				onChange={onChange}
				placeholder={placeholder}
			/>
			<Form.Control.Feedback type="invalid">
				{error}
			</Form.Control.Feedback>
		</Form.Group>
	);
};
