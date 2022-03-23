import cn from "classnames";
import React from "react";
import { Form } from "react-bootstrap";

interface TextFieldProps {
	name: string;
	label: string;
	className?: string;
	error?: string | false | undefined;
	value?: string;
	onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export const TextField = ({
	name,
	label,
	className,
	error,
	value,
	onChange,
}: TextFieldProps) => {
	return (
		<Form.Group className={cn(className, "mb-3")}>
			<Form.Label>{label}</Form.Label>
			<Form.Control
				name={name}
				type="text"
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
