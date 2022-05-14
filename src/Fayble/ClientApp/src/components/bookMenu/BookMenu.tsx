import { Dropdown } from "react-bootstrap";

interface BookMenuProps {}

export const BookMenu = ({}: BookMenuProps) => {
	return (
		<>
			<Dropdown.Item href="#/action-1">Action</Dropdown.Item>
			<Dropdown.Item href="#/action-2">Another action</Dropdown.Item>
			<Dropdown.Item href="#/action-3">Something else</Dropdown.Item>
		</>
	);
};
