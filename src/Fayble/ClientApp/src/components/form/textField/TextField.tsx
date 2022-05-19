import cn from "classnames";
import { FieldLock } from "components/fieldLock";
import React, { useState } from "react";
import { Form, InputGroup } from "react-bootstrap";

interface TextFieldProps {
	name: string;
	label?: string;
	className?: string;
	error?: string | false | undefined;
	value?: string;
	secure?: boolean;
	placeholder?: string;
	lockable?: boolean;
	locked?: boolean;
	onBlur?: (event: React.FocusEvent<HTMLInputElement>) => void;
	onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
	onLock?: (locked: boolean) => void;
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
	onLock,
	lockable = true,
	locked = false,
}: TextFieldProps) => {
	const [led, setLocked] = useState<boolean>(false);

	return (
		<Form.Group className={cn(className, "mb-3")}>
			{label && <Form.Label>{label}</Form.Label>}
			<InputGroup>
				<Form.Control
					name={name}
					type={secure ? "password" : "text"}
					isInvalid={!!error}					
					value={value}
					onBlur={onBlur}
					onChange={onChange}					
					placeholder={placeholder}
				/>
				<FieldLock
					locked={led}
					onClick={(a) => {
						setLocked(a);
					}}
				/>
			</InputGroup>
			<Form.Control.Feedback type="invalid">
				{error}
			</Form.Control.Feedback>
		</Form.Group>
	);
};
function usEffect(arg0: () => void, arg1: boolean[]) {
	throw new Error("Function not implemented.");
}
