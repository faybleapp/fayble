import cn from "classnames";
import { FieldLock } from "components/fieldLock";
import { LockableField } from "models/ui-models";
import React from "react";
import { Form, InputGroup } from "react-bootstrap";
import { useFormContext, useWatch } from "react-hook-form";

interface TextAreaFieldProps extends LockableField {
  name: string;
  label: string;
  className?: string;
  rows: number;
  placeholder?: string;
  lockable?: boolean;
}

export const TextAreaField: React.FC<TextAreaFieldProps> = ({
  name,
  label,
  className,
  rows,
  placeholder,
  lockable,
}) => {
  const {
    register,
    setValue,
    control,
    formState: { errors },
  } = useFormContext();

  const locked = useWatch({ control, name: `fieldLocks.${name}` });
  const field = register(name);
  return (
    <Form.Group className={cn(className, "mb-3")}>
      {label && <Form.Label>{label}</Form.Label>}
      <InputGroup>
        <Form.Control
          rows={rows}
          as="textarea"
          isInvalid={!!errors.name}
          {...field}
          onChange={(e: any) => {
            if (lockable) {
              setValue(`fieldLocks.${name}`, true, { shouldDirty: true });
            }
            field.onChange(e);
          }}
          placeholder={placeholder}
        />
        {lockable && (
          <FieldLock
            locked={locked}
            onClick={() =>
              setValue(`fieldLocks.${name}`, !locked, { shouldDirty: true })
            }
          />
        )}
        <Form.Control.Feedback type="invalid">
          {errors.name?.message?.toString()}
        </Form.Control.Feedback>
      </InputGroup>
    </Form.Group>
  );
};
