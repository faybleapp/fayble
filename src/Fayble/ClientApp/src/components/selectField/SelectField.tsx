import cn from "classnames";
import { OptionTypeBase, SelectFieldOption } from "models/ui-models";
import Select, { StylesConfig } from "react-select";
import styles from "./SelectField.module.scss";

export interface SelectFieldProps {
  name: string;
  label?: string;
  className?: string;
  creatable?: boolean;
  disabled?: boolean;
  clearable?: boolean;
  searchable?: boolean;
  value?: string;
  placeholder?: string;
  options: SelectFieldOption[] | [];
  onChange: (selectedValues: string) => void;
}

export const SelectField = ({
  creatable,
  disabled,
  className,
  clearable,
  searchable,
  value,
  options,
  onChange,
}: SelectFieldProps) => {
  const selectStyle: StylesConfig<OptionTypeBase, boolean> = {
    control: (provided) => ({
      ...provided,
      backgroundColor: styles.background,
      border: styles.border,
    }),
    input: (provided) => ({
      ...provided,
      color: styles.color,
    }),
    option: (provided) => ({
      ...provided,
      backgroundColor: styles.secondaryBackground,
      color: styles.color,
      ":hover": {
        color: styles.accentColor,
      },
      ":active": {
        background: styles.none,
      },
    }),
    singleValue: (provided) => ({
      ...provided,
      color: styles.color,
    }),
    valueContainer: (provided) => ({
      ...provided,
      backgroundColor: styles.background,
    }),
    menuList: (provided) => ({
      ...provided,
      backgroundColor: styles.secondaryBackground,
    }),
  };

  return (
    <Select
      isDisabled={disabled}
      className={cn(className, styles.select)}
      styles={selectStyle}
      isClearable={clearable === true}
      isSearchable={searchable === true}
      value={options.find((option) => option.value === value)}
      options={options}
      onChange={(option) => {
        onChange(option?.value);
      }}
    />
  );
};
