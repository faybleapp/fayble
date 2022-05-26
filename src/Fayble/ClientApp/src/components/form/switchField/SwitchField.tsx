import React from "react";
import { Form } from "react-bootstrap";

interface SwitchFieldProps {
	name: string;
	label: string;
	className?: string;
	value?: boolean;
	onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export const SwitchField = ({
	name,
	label,
	className,
	value,
	onChange,
}: SwitchFieldProps) => {
	return (		
			<Form.Check
				type="switch"
                checked={value}
                onChange={onChange}
				name={name}
                className={className}
				label={label}
			/>                             
		
	);
};
