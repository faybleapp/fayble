import { FieldLock } from "components/fieldLock";
import React from "react";
import { Form, InputGroup } from "react-bootstrap";

interface NumberFieldProps {
	name: string;
	label: string;
	className?: string;
	error?: string | false | undefined;
	value?: number;
	locked?: boolean;
	onLock?: (locked: boolean) => void;
	onChange: (event: React.ChangeEvent<unknown>) => void;
}

export const NumberField = ({
	name,
	label,
	className,
	error,
	value,
	locked = false,
	onChange,
	onLock,
}: NumberFieldProps) => {
	return (
		<Form.Group className={className}>
			<Form.Label>{label}</Form.Label>
			<InputGroup>
				<Form.Control
					name={name}
					type="number"
					isInvalid={!!error}
					value={value}
					onChange={(e: any) => {
						if (onLock) {
							onLock(true);
						}
						onChange(e);
					}}
				/>
				{onLock && <FieldLock locked={locked} onClick={onLock} />}
			</InputGroup>
			<Form.Control.Feedback type="invalid">
				{error}
			</Form.Control.Feedback>
		</Form.Group>
	);
};
