import cn from "classnames";
import { FieldLock } from "components/fieldLock";
import { LockableField } from "models/ui-models";
import React from "react";
import { Form, InputGroup } from "react-bootstrap";


interface OldTextFieldProps extends LockableField {
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

export const OldTextField = ({
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
	locked = false,
}: OldTextFieldProps) => {
	return (
		<Form.Group className={cn(className, "mb-3")}>
			{label && <Form.Label>{label}</Form.Label>}
			<InputGroup>
				<Form.Control
					name={name}
					type={secure ? "password" : "text"}
					isInvalid={!!error}
					value={value || ""}
					onBlur={onBlur}
					onChange={(e: any) => {
						if (onLock) {
							onLock(true);
						}
						onChange(e);
					}}
					placeholder={placeholder}
				/>
				{onLock && <FieldLock locked={locked} onClick={onLock} />}
			</InputGroup>
			<Form.Control.Feedback type="invalid">
				{error}
			</Form.Control.Feedback>
		</Form.Group>
	);
};
