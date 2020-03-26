import React, { useContext } from 'react'
import { RootStoreContext } from '../../app/stores/rootStore';
import { Menu, Container, Button, Dropdown, Image } from 'semantic-ui-react';
import { NavLink, Link } from 'react-router-dom';
import { observer } from 'mobx-react-lite';

function Navbar() {
    const rootStore = useContext(RootStoreContext)
    const { user, logout } = rootStore.userStore
    return (
        <Menu fixed="top" inverted>
            <Container>
                <Menu.Item header as={NavLink} exact to='/'>
                    <img src='/assets/logo.png' alt='logo' style={{ marginRight: '10px' }} />
            Jamaica Inspired Event Manager
        </Menu.Item>
                <Menu.Item name='Event Activities' as={NavLink} to='/events' />
                <Menu.Item>
                    <Button positive content='Create a Event' as={NavLink} to='/createEvent' />
                </Menu.Item>
                {user &&
                    <Menu.Item position='right'>
                        <Image avatar spaced='right' src={user.image || '/assets/user.png'} />
                        <Dropdown pointing='top left' text={user.displayName}>
                            <Dropdown.Menu>
                                <Dropdown.Item as={Link} to={`/profile/username`} text='My profile' icon='user' />
                                <Dropdown.Item onClick={logout} text='Logout' icon='power' />
                            </Dropdown.Menu>
                        </Dropdown>
                    </Menu.Item>
                }
            </Container>
        </Menu>
    );
};

export default observer(Navbar);
