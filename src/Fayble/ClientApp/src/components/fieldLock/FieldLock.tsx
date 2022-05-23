import { faLock, faUnlock } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import { InputGroup } from "react-bootstrap";
import styles from "./FieldLock.module.scss";
interface FieldLockProps {
	locked: boolean;
	onClick: (locked: boolean) => void;
}

export const FieldLock = ({ locked, onClick }: FieldLockProps) => {
	return (
		<InputGroup.Text onClick={() => onClick(!locked)}
			className={cn(styles.lock, { [styles.locked]: locked, [styles.unlocked]: !locked })}>
			<FontAwesomeIcon icon={locked ? faLock : faUnlock} />
		</InputGroup.Text>
	);
};
