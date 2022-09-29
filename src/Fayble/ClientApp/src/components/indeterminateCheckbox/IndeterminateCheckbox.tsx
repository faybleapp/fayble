import React, { useRef } from "react";

export enum IndeterminateCheckboxValue {
  Unchecked,
  Checked,
  Indeterminate,
}

interface IndeterminateCheckboxProps {
  value: IndeterminateCheckboxValue;
  onChange: () => void;
  className?: string;
}

export const IndeterminateCheckbox = ({
  value,
  onChange,
  className
}: IndeterminateCheckboxProps) => {
  const checkboxRef = useRef<HTMLInputElement>(null)

  React.useEffect(() => {
    if (value === IndeterminateCheckboxValue.Checked) {
      checkboxRef.current!.checked = true;
      checkboxRef.current!.indeterminate = false;
    } else if (value === IndeterminateCheckboxValue.Unchecked) {
      checkboxRef.current!.checked = false;
      checkboxRef.current!.indeterminate = false;
    } else if (value === IndeterminateCheckboxValue.Indeterminate) {
      checkboxRef.current!.checked = false;
      checkboxRef.current!.indeterminate = true;
    }
  }, [value]);

  return (
    <input className={className} type="checkbox" onChange={onChange} ref={checkboxRef} />
  );
};